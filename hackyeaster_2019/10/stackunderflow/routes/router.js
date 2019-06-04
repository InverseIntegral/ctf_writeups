const express = require('express');
const ObjectID = require("mongodb").ObjectID;
const router = express.Router();

const mongodb = require('./mongodb');
const {questions, users} = mongodb;

isAdmin = (req) => {
    if (req.session.username) {
        return req.session.username === 'the_admin';
    } else {
        return false;
    }
};

router.get('/', (req, res) => {
    res.render('index', {active: 'home', username: req.session.username});
});

router.get('/login', (req, res) => {
    res.render('login', {active: 'login', username: req.session.username});
});

router.post('/login', (req, res, error) => {
    users((collection) => {
        collection.findOne({
            username: req.body.username,
            password: req.body.password
        }, (err, item) => {
            if (!item) {
                error({message: 'Unknown user'});
                return;
            }

            req.session.username = req.body.username;
            res.redirect('/');
        });
    });
});

router.get('/logout', (req, res) => {
    req.session = null;
    res.redirect('/');
});

router.get('/questions', (req, res) => {
    questions((collection) => {
        collection.find({
            'hidden': {
                $ne: !isAdmin(req)
            }
        }).toArray((err, items) => {

            // Sort by answer length
            items = items.sort((a, b) => {
                return b.answers.length - a.answers.length;
            });

            res.render('questions', {questions: items, active: 'questions', username: req.session.username});
        });
    });
});

router.get('/questions/:id', (req, res, error) => {
    let id;

    try {
        id = new ObjectID(req.params.id);
    } catch (_) {
        error({message: 'That doesn\'t look like an id'});
        return;
    }

    questions((collection) => {
        collection.findOne({
            _id: id,
            'hidden': {
                $ne: !isAdmin(req)
            }
        }, (err, item) => {
            if (!item) {
                error({message: 'Unknown question!'});
                return;
            }

            res.render('question', {question: item, active: 'questions', username: req.session.username});
        });
    });
});

router.get('/robots.txt', (req, res) => {
    res.render('robots');
});

module.exports = router;
