const MongoClient = require('mongodb').MongoClient;
const url = 'mongodb://mongo:27017';
const dbName = 'stackunderflow';

function collection(name) {
    return (callback) => {
        MongoClient.connect(url, {useNewUrlParser: true}, function (error, client) {
            const collection = client.db(dbName).collection(name);
            callback(collection);
            client.close();
        });
    }
}

module.exports = {
    questions: collection('questions'),
    users: collection('users')
};
