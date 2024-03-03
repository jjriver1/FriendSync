// import axios using require
const axios = require('axios');

// create a new axios instance
const instance = axios.create({
    baseURL: 'http://localhost:5180',
});

// do a get request at the /api/User endpoint

async function createUser(userObject) {
    try {
        return await instance.post(`/api/User/`, userObject);
    } catch (error) {
        return error.response;
    }
}

async function createUserMock() {
    let promise = await createUser({
        adf: "string",
        email: "string",
        password: "string",
        bio: "string"
    });

    if (promise.status === 201) {
        console.log("Request succeeded");
    } else {
        console.log("Request failed " + promise.status + " " + promise.statusText + " " + promise.data.message);
    }
}

// noinspection JSIgnoredPromiseFromCall
createUserMock();
