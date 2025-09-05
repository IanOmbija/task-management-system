import { createTheme } from "@mui/material/styles";

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: { main: "#72cbcc" },
    secondary: { main: "#9c27b0" },
  },
  shape: { borderRadius: 20 },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          background: "linear-gradient(135deg,rgb(237, 244, 244) 50%,rgb(255, 250, 255) 40%)",
          backgroundAttachment: "fixed",
          minHeight: "100vh",
        },
      },
    },
  },
});



export default theme;
