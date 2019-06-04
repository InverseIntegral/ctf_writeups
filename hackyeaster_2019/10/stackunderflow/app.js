const createError = require('http-errors');
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const session = require('express-session');
const logger = require('morgan');
const hbs = require('hbs');

hbs.registerHelper('eq', function (a, b, options) {
    if (a === b) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
});

const router = require('./routes/router');

const mongodb = require('./routes/mongodb');
const {questions, users} = mongodb;

const defaultQuestions = require('./questions.json');
const defaultUsers = require('./users.json');

questions((collection) => {
    collection.deleteMany({});
    collection.insertMany(defaultQuestions);
});

users((collection) => {
    collection.deleteMany({});
    collection.insertMany(defaultUsers)
});

const app = express();

app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'hbs');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());

app.use(session({
    resave: false,
    saveUninitialized: false,
    unset: 'destroy',
    secret: ['!THFEnaHk6a?dysf9qQa_Wl`thAcEj6rh3']
}));

app.use(express.static(path.join(__dirname, 'public')));

app.use('/', router);

app.use(function(req, res, next) {
  next(createError(404));
});

app.use((err, req, res, next) => {
  res.locals.message = err.message;
  res.locals.error = {};

  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
