import axios from 'axios';

const API = axios.create({

    baseURL: 'http://localhost:5213/api'

});

export default API;