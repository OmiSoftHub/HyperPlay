'use client'

import { useRouter } from "next/navigation";
import { useAuth } from "./context/AuthContext";
import { useState, useEffect } from "react";

const PrivateRoute = ({ children }) => {
  const { token, decodedToken } = useAuth();
  const router = useRouter();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (token && decodedToken) {
      setLoading(false);
    } else if (!token) {
      setLoading(false); 
    }
  }, [token, decodedToken]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!token || decodedToken?.role !== "Admin") {
    router.push("/error");
    return null;
  }

  return children;
};

export default PrivateRoute;