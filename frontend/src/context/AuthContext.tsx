import React, {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from 'react';
import { AuthResponse } from '../types';
import { attachInterceptors } from '../services/api';

const STORAGE_KEY = 'tms_auth';

type AuthState = {
  token: string | null;
  userId: string | null;
  username: string | null;
  role: 'USER' | 'ADMIN' | null;
  expiresAt: string | null;
};

type AuthContextValue = AuthState & {
  login: (resp: AuthResponse) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  
  const [state, setState] = useState<AuthState>(() => {
    try {

      const raw = localStorage.getItem(STORAGE_KEY);

      return raw
        ? (JSON.parse(raw) as AuthState)
        : { token: null, userId: null, username: null, role: null, expiresAt: null };
    } catch {
      return { token: null, userId: null, username: null, role: null, expiresAt: null };
    }
  });

  const tokenRef = useRef<string | null>(state.token);
  const [hydrated, setHydrated] = useState(false);

  const logout = useCallback(() => {
    setState({ token: null, userId: null, username: null, role: null, expiresAt: null });
    tokenRef.current = null;
    localStorage.removeItem(STORAGE_KEY);
  }, []);

  const login = useCallback((resp: AuthResponse) => {
    const next: AuthState = {
      token: resp.token,
      userId: resp.userId,
      username: resp.username,
      role: resp.role,
      expiresAt: resp.expiresAt,
    };
    setState(next);
    tokenRef.current = next.token;
    localStorage.setItem(STORAGE_KEY, JSON.stringify(next));
  }, []);

  
  useEffect(() => {
    attachInterceptors(() => tokenRef.current, logout);
  }, [logout]);

  
  useEffect(() => {
    tokenRef.current = state.token;
    setHydrated(true);
  }, [state.token]);

  
  const timeoutRef = useRef<number | null>(null);
  useEffect(() => {
    if (timeoutRef.current) window.clearTimeout(timeoutRef.current);

    if (state.expiresAt) {
      const ms = new Date(state.expiresAt).getTime() - Date.now();
      console.log("Compared the expiry time:-", ms);
      if (ms > 0) {
        timeoutRef.current = window.setTimeout(logout, ms);
      } else {
        logout();
      }
    }

    return () => {
      if (timeoutRef.current) window.clearTimeout(timeoutRef.current);
    };
  }, [state.expiresAt, logout]);

  const value = useMemo(() => ({ ...state, login, logout }), [state, login, logout]);

  // hydration before rendering children
  if (!hydrated) return null;
  console.log("Allows the token to be loaded first");
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within an AuthProvider');
  return ctx;
}
