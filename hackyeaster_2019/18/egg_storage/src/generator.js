var Binaryen = require('binaryen');
var module = new Binaryen.Module();

const flag = '2929dac4326ad3553872c6a7';
console.log(`Flag: ${flag}`);

function xorEncrypt(plain, password) {
    const cipher = new Array(plain.length);

    for (let i = 0; i < plain.length; i++) {
        cipher[i] = plain[i].charCodeAt(0) ^ password[i].charCodeAt(0);
    }

    return cipher;
}

const password = 'Th3P4r4d0X0fcH01c3154L13';
const encryptedFlag = xorEncrypt(flag, password);

password.split('').map(e => e.charCodeAt(0)).forEach((e, i) => {
    console.log(i + ", " + e);
});

console.log(`Encrypted flag: ${encryptedFlag}`);

function createCheckRangeFunction() {
    const validateRangeType = module.addFunctionType('validateRangeType', Binaryen.i32, [Binaryen.i32]);

    const valid = [48, 49, 51, 52, 53, 72, 76, 88, 99, 100, 102, 114];
    const instructions = [];

    const compares = [];

    for (let e of valid) {
        compares.push(module.i32.eq(module.i32.const(e), module.get_local(0, Binaryen.i32)));
    }

    let condition = module.i32.or(compares[0], compares[1]);

    for (let j = 2; j < 12; j++) {
        condition = module.i32.or(condition, compares[j]);
    }

    instructions.push(module.if(condition, module.return(module.i32.const(1))));
    instructions.push(module.return(module.i32.const(0)));

    module.addFunction('validateRange', validateRangeType, [], module.block(null, instructions));
    module.addFunctionExport('validateRange', 'validateRange');
}

createCheckRangeFunction();

function createValidateFunction(length) {
    let i32Type = Binaryen.i32;
    let i32 = module.i32;

    const validatePasswordType = module.addFunctionType('validatePasswordType', i32Type, new Array(length).fill(i32Type));
    const instructions = [];

    instructions.push(module.drop(module.call('nope', [], i32Type)));

    for (let i = 0; i < length; i++) {
        instructions.push(i32.store8(i, 0, module.i32.const(24), module.get_local(i, i32Type)));
    }

    instructions.push(module.set_local(24, i32.const(4)));

    const validationLoop = module.loop('validation', module.block(null, [
        module.if(i32.eqz(module.call('validateRange', [i32.load8_u(0, 0, i32.add(i32.const(24), module.get_local(24, i32Type)))], i32Type)), module.return(i32.const(0))),
        module.set_local(24, i32.add(module.get_local(24, i32Type), i32.const(1))),
        module.br_if('validation', i32.le_s(module.get_local(24, i32Type), i32.const(length - 1)))
    ]));

    instructions.push(validationLoop);

    instructions.push(module.if(i32.ne(module.get_local(0, i32Type), i32.const('T'.charCodeAt(0))), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(1, i32Type), i32.const('h'.charCodeAt(0))), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(2, i32Type), i32.const('3'.charCodeAt(0))), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(3, i32Type), i32.const('P'.charCodeAt(0))), module.return(i32.const(0))));

    instructions.push(module.if(i32.ne(module.get_local(23, i32Type), module.get_local(17, i32Type)), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(12, i32Type), module.get_local(16, i32Type)), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(22, i32Type), module.get_local(15, i32Type)), module.return(i32.const(0))));

    const condition1 = i32.ne(i32.sub(module.get_local(5, i32Type), module.get_local(7, i32Type)), i32.const(14));
    instructions.push(module.if(condition1, module.return(i32.const(0))));

    const condition2 = i32.ne(i32.add(module.get_local(14, i32Type), i32.const(1)), module.get_local(15, i32Type));
    instructions.push(module.if(condition2, module.return(i32.const(0))));

    const condition3 = i32.ne(i32.rem_s(module.get_local(9, i32Type), module.get_local(8, i32Type)), i32.const(40));
    instructions.push(module.if(condition3, module.return(i32.const(0))));

    const condition4 = i32.ne(i32.add(i32.sub(module.get_local(5, i32Type), module.get_local(9, i32Type)), module.get_local(19, i32Type)), i32.const(79));
    instructions.push(module.if(condition4, module.return(i32.const(0))));

    const condition5 = i32.ne(i32.sub(module.get_local(7, i32Type), module.get_local(14, i32Type)), module.get_local(20, i32Type));
    instructions.push(module.if(condition5, module.return(i32.const(0))));

    const condition6 = i32.ne(i32.mul(i32.rem_s(module.get_local(9, i32Type), module.get_local(4, i32Type)), i32.const(2)), module.get_local(13, i32Type));
    instructions.push(module.if(condition6, module.return(i32.const(0))));

    const condition7 = i32.ne(i32.rem_s(module.get_local(13, i32Type), module.get_local(6, i32Type)), i32.const(20));
    instructions.push(module.if(condition7, module.return(i32.const(0))));

    const condition8 = i32.ne(i32.rem_s(module.get_local(11, i32Type), module.get_local(13, i32Type)), i32.sub(module.get_local(21, i32Type), i32.const(46)));
    instructions.push(module.if(condition8, module.return(i32.const(0))));

    const condition9 = i32.ne(i32.rem_s(module.get_local(7, i32Type), module.get_local(6, i32Type)), module.get_local(10, i32Type));
    instructions.push(module.if(condition9, module.return(i32.const(0))));

    const condition10 = i32.ne(i32.rem_s(module.get_local(23, i32Type), module.get_local(22, i32Type)), i32.const(2));
    instructions.push(module.if(condition10, module.return(i32.const(0))));

    instructions.push(module.set_local(24, i32.const(4)));
    instructions.push(module.set_local(25, i32.const(0)));
    instructions.push(module.set_local(26, i32.const(0)));

    const checksumLoop = module.loop("checksum", module.block(null, [
        module.set_local(25, i32.add(module.get_local(25, i32Type), i32.load8_u(0, 0, i32.add(i32.const(24), module.get_local(24, i32Type))))),
        module.set_local(26, i32.xor(module.get_local(26, i32Type), i32.load8_u(0, 0, i32.add(i32.const(24), module.get_local(24, i32Type))))),

        module.set_local(24, i32.add(module.get_local(24, i32Type), i32.const(1))),
        module.br_if("checksum", i32.le_s(module.get_local(24, i32Type), i32.const(length)))
    ]));

    instructions.push(checksumLoop);

    instructions.push(module.if(i32.ne(module.get_local(25, i32Type), i32.const(1352)), module.return(i32.const(0))));
    instructions.push(module.if(i32.ne(module.get_local(26, i32Type), i32.const(44)), module.return(i32.const(0))));

    instructions.push(module.drop(module.call('decrypt', [], i32Type)));
    instructions.push(module.return(i32.const(1)));

    module.addFunction('validatePassword', validatePasswordType, [i32Type, i32Type, i32Type], module.block(null, instructions));
    module.addFunctionExport('validatePassword', 'validatePassword');
}

createValidateFunction(flag.length);

function createDecryptionFunction(length) {
    let i32Type = Binaryen.i32;
    let i32 = module.i32;

    const decryptType = module.addFunctionType('decryptionType', i32Type, []);
    const instructions = [];

    const address1 = module.get_local(0, i32Type);
    const address2 = i32.add(i32.const(24), module.get_local(0, i32Type));

    const decryptionLoop = module.loop("decryption", module.block(null, [
        i32.store8(0, 0, module.get_local(0, i32Type), i32.xor(i32.load8_u(0, 0, address1), i32.load8_u(0, 0, address2))),
        module.set_local(0, i32.add(module.get_local(0, i32Type), i32.const(1))),
        module.br_if("decryption", i32.le_s(module.get_local(0, i32Type), i32.const(length)))
    ]));

    instructions.push(decryptionLoop);
    instructions.push(module.return(i32.const(1337)));

    module.addFunction('decrypt', decryptType, [i32Type], module.block(null, instructions));
    module.addFunctionExport('decrypt', 'decrypt');
}

createDecryptionFunction(flag.length);

module.addFunctionImport('nope', 'base', 'functions', module.addFunctionType('nope', Binaryen.i32, []));

module.setMemory(1, 1, '0', [{
    offset: module.i32.const(0),
    data: encryptedFlag
}]);

module.validate();
// module.optimize();

process.stdout.write('[');
module.emitBinary().forEach(e => process.stdout.write(e + ','));
process.stdout.write(']');

module.dispose();
