# 16 - Santa's Crypto Vault

## Description

With the recent Crypto Rally, Santa has invested all his funds into Santa Coins. Because he doesn't trust any existing
software to securely store his wallet, he asked one of his elves, "Mikitaka Hazekura", to implement their own crypto
vault using enterprise software design patterns, the latest technology and thorough unit tests. They're so proud of it,
they've decided to open source it!

Santa requested to use multiple words, based off his favorite anime, instead of one long password to make it more
memorable and secure at the same time.

## Solution

For this challenge we are given the Kotlin source code of a web service. The important part is the vault service that
compares the user-supplied that with the configured values:

```kotlin
@Service
internal class DefaultVaultService(private val hashService: HashService, properties: VaultProperties) : VaultService {
    private val secret = properties.secret

    @Volatile
    private var matched: Boolean = false

    private suspend fun String.hash(): String = hashService.hash(this)

    override suspend fun checkCode(code: VaultCode): Boolean {
        matched = true
        if (code.bestCharacter.hash() != secret.bestCharacter) {
            log.warn { "Wrong bestCharacter '${code.bestCharacter}', rejecting code" }
            matched = false
        }
        if (code.bestWaifu.hash() != secret.bestWaifu) {
            log.warn { "Wrong bestWaifu '${code.bestWaifu}', rejecting code" }
            matched = false
        }
        if (code.reliableGuy.hash() != secret.reliableGuy) {
            log.warn { "Wrong reliableGuy '${code.reliableGuy}', rejecting code" }
            matched = false
        }
        if (code.bestStand.hash() != secret.bestStand) {
            log.warn { "Wrong bestStand '${code.bestStand}', rejecting code" }
            matched = false
        }
        if (code.bestVillain.hash() != "Dio".hash()) { // TODO move hashed value to configuration
            log.warn { "Wrong bestVillain '${code.bestVillain}', rejecting code" }
            matched = false
        }

        log.info { "Matched code = $matched" }

        return matched
    }
}
```

Our goal is to get the service to return `true`. The following test case gives us a hint on how we can achieve this:

```kotlin
@Test
@Disabled("TODO sometimes this test fails and a dummyCode passes, hopefully just a test issue")
fun `parallel execution works`() = runBlocking {
    listOf(
        async { controller.check(dummyCode) },
        // Hint: This delay needs to be adjusted based on computer speed if you want to run the test locally
        async { delay(375.milliseconds); controller.check(dummyCode) },
    ).map {
        it.await()
    }.forEach {
        Assertions.assertEquals(
            HttpStatus.FORBIDDEN,
            it.statusCode
        )
    }
}
```

If the service is called twice with the right delay, then the service sometimes returns true even though the values are
incorrect. This is because `matched` is a volatile, shared variable. The first service call reaches the return
statement while the second service call concurrently sets the variable to `true`. To exploit this I simply opened two
tabs in the browser and submitted the requests quickly after each other. After a few tries I got the flag
`HV21{c0ncurrency_1s_a_b1tch}`.

