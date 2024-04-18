/* eslint-disable @typescript-eslint/no-explicit-any */

import axios, { AxiosError, AxiosResponse, AxiosResponseHeaders } from "axios";

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

function generateAxiosResponse(): AxiosResponse {
  return {
    status: 0,
    statusText: "",
    data: "",
    headers: {} as AxiosResponseHeaders,
    config: {
      url: "",
      method: "",
      headers: {} as AxiosResponseHeaders,
    },
  };
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
/**
 * Handle an error of any type and return an AxiosResponse for easier handling
 * @param error The error to handle
 * @returns An AxiosResponse object with the useful error information
 */
function handleError(error: any): AxiosResponse<any, any> {
  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError;

    if (axiosError.response) {
      return axiosError.response;
    }

    // If the error does not have a response, return a generic response
  }

  const response = generateAxiosResponse();
  response.status = 503;
  response.statusText = "Service Unavailable";
  response.data = "Unknown error occurred";

  if (error instanceof Error) {
    response.data = error.message;
    return response;
  }

  return response;
}

export async function createUser(
  userObject: User,
): Promise<AxiosResponse<any, any>> {
  try {
    userObject.id = "";
    return await instance.post(`/api/User/`, userObject);
  } catch (error) {
    return handleError(error);
  }
}

export async function login(
	email: string,
	password: string,
): Promise<AxiosResponse<any, any>> {
	try {
		return await instance.post(`/api/User/login/${email}/${password}`);
	} catch (error) {
		return handleError(error);
	}
}

export type { User };
