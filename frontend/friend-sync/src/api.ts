// import axios using require
import axios, { AxiosError } from "axios";

// create a new axios instance
const instance = axios.create({
  baseURL: "http://localhost:5180",
});

interface User {
  id: string;
  username: string;
  email: string;
  password: string;
  bio: string;
}

// eslint-disable-next-line @typescript-eslint/no-unused-vars,@typescript-eslint/explicit-module-boundary-types
export default async function createUser(userObject: User) {
  try {
    userObject.id = "";
    return await instance.post(`/api/User/`, userObject);
  } catch (error) {
    if (error instanceof Error) {
      const axiosError = error as AxiosError;
      return axiosError.response;
    }
    return null;
  }
}

export type { User };
