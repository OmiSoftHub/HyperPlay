'use client'

import { createContext, useState, useContext, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "./AuthContext";
import { createReserve, getReserveDetails, confirmReserve } from "../endpoints/ReserveEndpoints";
import { getVarLS, deleteLocalStorage } from "../utils/keep";

import { CREATE_RESERVE, CONFIRM_RESERVE } from "../config";

// Crear el contexto de checkout
const CheckoutContext = createContext();

// Proveedor del contexto
export function CheckoutProvider({ children }) {
  const { token } = useAuth();
  const [modeOfPay, setModeOfPay] = useState(0);
  const [reserve, setReserve] = useState([]);
  const [reserveId, setReserveId] = useState(null);
  const [message, setMessage] = useState("");
  const [orderId, setOrderId] = useState(null);
  const router = useRouter();

  // Initialize state from localStorage only on client side
  useEffect(() => {
    try {
      const storedOrderId = localStorage.getItem("orderId");
      if (storedOrderId) {
        setOrderId(storedOrderId);
      }
    } catch (error) {
      console.error("Error accessing localStorage:", error);
    }
  }, []);

  const handleCreateReserve = async (modeOfPay, useLocalReserve = false) => {
    try {
      let reserve;

      if (useLocalReserve) {
        reserve = getVarLS("reserve");
      } else {
        const cart = getVarLS("cart");
        if (!cart || !cart.items || cart.items.length === 0) {
          setMessage("El carrito está vacío. No se puede crear una reserva.");
          return null;
        }
        reserve = { ...cart };
      }

      if (!reserve || !reserve.items || reserve.items.length === 0) {
        setMessage("No se encontró una reserva válida.");
        console.error("No se encontró una reserva válida.", reserve);
        return null;
      }

      const reserveData = {
        modeOfPay,
        reserve,
      };

      if (!token) {
        return null;
      }

      const response = await createReserve(CREATE_RESERVE, reserveData, token);

      if (typeof response === "number") {
        setReserveId(response);
        return response;
      } else {
        throw new Error("Respuesta inesperada del backend.");
      }
    } catch (error) {
      console.error("Error al crear la reserva:", error);
      setMessage(`Error: ${error.message}`);
      throw error;
    }
  };

  const loadReserveDetails = async (url, reserveId) => {
    if (!token) {
      console.error("No se puede cargar la reserva: Token de autenticación no disponible.");
      return { items: [] };
    }

    try {
      const data = await getReserveDetails(url, reserveId, token);

      if (JSON.stringify(data) !== JSON.stringify(reserve)) {
        setReserve(data);
      }

      return data;
    } catch (error) {
      console.error("Error al cargar los detalles de la reserva:", error.message);
      setReserve({ items: [] });
      throw error;
    }
  };

  const handleConfirmReserve = async (endpoint, reserveId) => {
    try {
      const data = await confirmReserve(endpoint, reserveId, token);
      setOrderId(data.id);
      try {
        localStorage.setItem("orderId", data.id);
      } catch (error) {
        console.error("Error saving to localStorage:", error);
      }
      setReserve([]);
      deleteLocalStorage("reserve");
      return data.id;
    } catch (error) {
      console.error("Error confirming reserve:", error);
      throw error;
    }
  };

  const value = {
    setModeOfPay,
    handleCreateReserve,
    reserve,
    message,
    reserveId,
    setReserveId,
    loadReserveDetails,
    handleConfirmReserve,
    orderId,
    setOrderId,
  };

  return (
    <CheckoutContext.Provider value={value}>
      {children}
    </CheckoutContext.Provider>
  );
}

export function useCheckout() {
  const context = useContext(CheckoutContext);
  if (!context) {
    throw new Error("useCheckout must be used within a CheckoutProvider");
  }
  return context;
}
