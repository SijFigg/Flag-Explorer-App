const baseUrl = process.env.API_BASE_URL || "http://localhost:8000";
console.log("API base URL:", baseUrl);
export const getAllCountries = async () => {
    const response = await fetch(`${baseUrl}/countries`);
    return await response.json();
};

export const getCountryByName = async (name) => {
    const response = await fetch(`${baseUrl}/countries/${encodeURIComponent(name)}`);
    return await response.json();
};
