import axios from "axios";
import { useEffect, useState } from "react"
const baseUrl = require("../URL.json").url
export const UseFetch = (url, preProccess = (data) => data) => {
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [data, setData] = useState(null);
    useEffect(() => {fetch()},[])
    const fetch = async () => {
        try {
            const resp = await axios.get(baseUrl + url);
            console.log('fetching')
            setData(preProccess(resp.data));
        } catch (error) {
            console.log(error);
            setError(error.message);
        } finally {
            setIsLoading(false);
        }
        
    }
    return [
        (data),
        isLoading,
        error
    ];
}