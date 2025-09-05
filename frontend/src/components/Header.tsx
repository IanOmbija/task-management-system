import React from "react";
import { AppBar, Toolbar, Typography, Box, Button } from "@mui/material";
import { useAuth } from "../context/AuthContext";

const Header: React.FC = () => {
  const { username, role, logout } = useAuth();
  return (
    <AppBar position="sticky" elevation={1}>
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Task Management
        </Typography>
        <Box sx={{ display: "flex", gap: 2, alignItems: "center" }}>
          {username && (
            <Typography variant="body2">
              {username} ({role})
            </Typography>
          )}
          <Button color="inherit" onClick={logout}>
            Logout
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
