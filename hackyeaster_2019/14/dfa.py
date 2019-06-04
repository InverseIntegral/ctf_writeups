import sys
import deadpool_dfa
import phoenixAES

def processinput(iblock, blocksize):
    return (bytes.fromhex('%0*x' % (2*blocksize, iblock)),None)

def processoutput(output, blocksize):
    idx = output.decode().find("encrypt: ") + 9
    res = output[idx:-1]
    return int(res, 16)

engine = deadpool_dfa.Acquisition(targetbin='./WhiteBoxTmp', targetdata='./WhiteBoxTmp', goldendata='WhiteBox', dfa=phoenixAES, processoutput=processoutput, processinput=processinput, encrypt=True, minleaf=0x1, minleafnail=0x1, maxleaf=0x100)
tracefiles=engine.run()

for tracefile in tracefiles[0]:
    if phoenixAES.crack_file(tracefile):
        break

