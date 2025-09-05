import React from 'react';
import {
  Drawer,
  Toolbar,
  Box,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  useMediaQuery,
} from '@mui/material';
import DashboardIcon from '@mui/icons-material/Dashboard';
import LogoutIcon from '@mui/icons-material/Logout';
import { NavLink, useNavigate } from 'react-router-dom';
import { useTheme } from '@mui/material/styles';
import { useAuth } from '../context/AuthContext';

const drawerWidth = 240;

interface SidebarProps {
  open: boolean;
  onClose: () => void;
}

/**
 * Sidebar navigation for the app.
 * Switches between permanent (desktop) and temporary (mobile/tablet) drawers.
 */
const Sidebar: React.FC<SidebarProps> = ({ open, onClose }) => {
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up('md'));

  const { logout } = useAuth();
  const navigate = useNavigate();

 
  const sidebarContent = (
    <Box role="presentation" sx={{ width: drawerWidth }}>
      <Toolbar /> 
      <List>
       
        <ListItemButton
          component={NavLink}
          to="/"
          end
          onClick={!isDesktop ? onClose : undefined}
          sx={{ '&.active': { bgcolor: 'action.selected' } }}
        >
          <ListItemIcon>
            <DashboardIcon />
          </ListItemIcon>
          <ListItemText primary="Dashboard" />
        </ListItemButton>

        {/* Logout action */}
        <ListItemButton
          onClick={() => {
            logout();
            navigate('/login');
          }}
        >
          <ListItemIcon>
            <LogoutIcon />
          </ListItemIcon>
          <ListItemText primary="Logout" />
        </ListItemButton>
      </List>
    </Box>
  );

  // Permanent drawer for desktop
  if (isDesktop) {
    return (
      <Drawer
        variant="permanent"
        open
        PaperProps={{
          sx: { width: drawerWidth, boxSizing: 'border-box' },
        }}
      >
        {sidebarContent}
      </Drawer>
    );
  }

  // Temporary drawer for mobile/tablet
  return (
    <Drawer
      variant="temporary"
      open={open}
      onClose={onClose}
      ModalProps={{ keepMounted: true }}
      PaperProps={{ sx: { width: drawerWidth } }}
    >
      {sidebarContent}
    </Drawer>
  );
};

export default Sidebar;
export { drawerWidth };
