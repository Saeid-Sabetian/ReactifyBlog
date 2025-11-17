# Swagger UI Theme Toggle

This project includes a custom light/dark theme toggle for the Swagger UI. The chosen theme is persistent across sessions using a cookie.

## Cookie Details
- **Name**: `swagger_theme`
- **Accepted Values**: `light` or `dark`
- **Default Behavior**: If the cookie is not present, the Swagger UI defaults to the `light` theme.

## How to Test
1.  Run the backend API project (`ReactifyBlog.Api`).
2.  Open your browser and navigate to the Swagger UI endpoint (e.g., `https://localhost:5001/swagger` or `http://localhost:5000/swagger`).
3.  Locate the theme toggle button in the top right corner of the Swagger UI (sun/moon icon).
4.  Click the button to switch between light and dark themes.
5.  Reload the page to verify that the last selected theme is loaded from the cookie.
6.  You can also inspect your browser's cookies to see the `swagger_theme` cookie being set and updated.
