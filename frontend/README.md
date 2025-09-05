# Task Management System - Frontend

This is the **React + MUI** frontend for the Task Management System. 

Integrated with a .NET backend for task management actions.

---

#### **Requirements**
- Node.js >= 18
- npm or yarn
- React
- Material-UI (MUI)
- Axios
- Context API


##  Features
- User Registration & Authentication (JWT)
- Role-based access control
- CRUD operations for tasks
- Task assignment to users
- API Integration via Axios with interceptors
- UI/UX built with Material-UI
- State Management using Context API

---

## Quick Set up and Run

```bash
# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build
```

---



The app will run at `http://localhost:3000` by default.


## Environment Variables

To run this project, you will need to add the following environment variables to your `.env` file in the `frontend/` folder:

```
REACT_APP_API_BASE_URL=http://localhost:5031/api
```

Or the URL that your backend API will be running on, for the example instance, it runs on the above `URL`

## Authors

- Developed by: [@IanOmbija](https://www.github.com/IanOmbija)

