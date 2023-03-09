/* const serverURL = "https://localhost:44393/api/"; */
const serverURL = `/api/`;
const poster = async (endpoint, dataToPost) => {
  let payload;
  let headers = buildHeaders();
  try {
    let response = await fetch(`${serverURL}${endpoint}`, {
      method: "POST",
      headers: headers,
      body: JSON.stringify(dataToPost),
    });
    payload = await response.json();
  } catch (error) {
    payload = error;
  }
  return payload;
};
export { poster };

const fetcher = async (endpoint) => {
  let payload;
  let headers = buildHeaders();
  try {
    let response = await fetch(`${serverURL}${endpoint}`, {
      method: "GET",
      headers: headers,
    });
    payload = await response.json();
  } catch (err) {
    console.log(err);
    payload = { error: `Error has occured: ${err.message}` };
  }
  return payload;
};
export { fetcher };

const buildHeaders = () => {
  const myHeaders = new Headers();
  const user = JSON.parse(sessionStorage.getItem("customer"));
  if (user) {
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authorization", "Bearer " + user.token);
  } else {
    myHeaders.append("Content-Type", "application/json");
  }
  return myHeaders;
};
