'use client'

import { useContext, useState, useEffect } from 'react';
import { CreateData } from '../../utils/dataCart';
import { ConvertToDecimal, TotalPrice } from '../../utils/price';
import { useRouter } from "next/navigation";
import { CartContext } from '../../context/CartContext';
import { CheckoutContext } from '../../context/CheckoutContext';
import Button from '../Buttons/Button';
import LoginModal from '../Login/LoginModal'; 
import RegisterModal from '../Register/RegisterModal';
import classes from './CartPayment.module.css';
import { useAuth } from '../../context/AuthContext';
import { getVarLS, getVarSessionStorage } from '../../utils/keep'

function CartPayment() {
  const { gameDetails, items } = useContext(CartContext);
  const { handleCreateReserve, setReserveId } = useContext(CheckoutContext); 
  const { token } = useAuth();
  const [data, setData] = useState([]);
  const [isLoginModalOpen, setIsLoginModalOpen] = useState(false);
  const [isRegisterModalOpen, setIsRegisterModalOpen] = useState(false); 
  const [isPaymentInitiated, setIsPaymentInitiated] = useState(false);
  const [paymentRoute, setPaymentRoute] = useState("");
  const [paymentMode, setPaymentMode] = useState(null);
  const [priceEthereum, setPriceEthereum] = useState(null);
  const router = useRouter();

  useEffect(() => {
    const updatedData = CreateData(items, gameDetails);
    setData(updatedData);
  }, [gameDetails, items]);

  useEffect(() => {
    if (token && isPaymentInitiated) {
      handleLoginSuccess();
    }
  }, [token, isPaymentInitiated]);

  const handlePaymentClick = async (useLocalReserve, modeOfPay, route, priceEthereum) => {
    if (!token) {
      setIsLoginModalOpen(true); 
      setIsPaymentInitiated(true); 
      setPaymentRoute(route); 
      setPaymentMode(modeOfPay);
      setPriceEthereum(priceEthereum);
      return;
    }

    try {
      const reserveId = await handleCreateReserve(useLocalReserve, modeOfPay);
      setReserveId(reserveId);
      router.push(route);
    } catch (error) {
      console.error("Error al procesar el pago:", error);
    }
  };

  const handleLoginSuccess = async () => {
    if (paymentRoute && paymentMode) {
      try {
        const reserveId = await handleCreateReserve(false, paymentMode);
        setReserveId(reserveId);
        router.push(paymentRoute);
      } catch (error) {
        console.error("Error al procesar el pago después del login:", error);
      }
    }
  };

  return (
    <div className={classes.cartPayment}>
      <div className={classes.total}>
        <h3>Total: {ConvertToDecimal(TotalPrice(data))}€</h3>
      </div>
      <div className={classes.buttons}>
        <Button
          variant="long"
          color="morado-azul"
          onClick={() => handlePaymentClick(false, "Stripe", "/checkout/stripe", null)}
        >
          Pagar con Stripe
        </Button>
        <Button
          variant="long"
          color="morado-azul"
          onClick={() => handlePaymentClick(false, "Ethereum", "/EthereumCheckout", TotalPrice(data))}
        >
          Pagar con Ethereum
        </Button>
      </div>
      {isLoginModalOpen && (
        <LoginModal
          onClose={() => setIsLoginModalOpen(false)}
          onRegisterClick={() => setIsRegisterModalOpen(true)}
          onSuccess={handleLoginSuccess}
        />
      )}
      {isRegisterModalOpen && (
        <RegisterModal
          onClose={() => setIsRegisterModalOpen(false)}
          onLoginClick={() => setIsLoginModalOpen(true)}
        />
      )}
    </div>
  );
}

export default CartPayment;