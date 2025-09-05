import React, { useState } from 'react';
import { Box, AppBar, Toolbar, IconButton, Typography } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { Outlet } from 'react-router-dom';

import Sidebar, { drawerWidth } from './Sidebar';
import { useAuth } from '../context/AuthContext';

const Layout: React.FC = () => {
  const [open, setOpen] = useState(false);
  const { username, role } = useAuth();

  const handleSidebarOpen = () => setOpen(true);
  const handleSidebarClose = () => setOpen(false);

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar 
        position="fixed" 
        sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            edge="start"
            onClick={handleSidebarOpen}
            sx={{ mr: 2, display: { md: 'none' } }}
          >
            <MenuIcon />
          </IconButton>

       
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Task Management System
          </Typography>

          {/* User Info */}
          {username && (
            <Typography variant="body2">
              {username} ({role})
            </Typography>
          )}
        </Toolbar>
      </AppBar>

      <Sidebar open={open} onClose={handleSidebarClose} />
      
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          mt: 8,
          width: { md: `calc(100% - ${drawerWidth}px)` },
        }}
      >
        <Outlet />
      </Box>
    </Box>
  );
};

export default Layout;
