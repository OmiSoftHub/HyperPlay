'use client'

import { useEffect, useState, useContext } from "react";
import classes from "./PaymentOrder.module.css";
import { orderById } from "../../endpoints/OrderEndpoints";
import { useAuth } from "../../context/AuthContext";
import { ORDER_BY_ID, BASE_URL } from "../../config";
import { CheckoutContext } from "../../context/CheckoutContext";
import Link from "next/link";
import { getVarLS, updateLocalStorage } from "../../utils/keep";

const paymentModes = {
  0: "Ethereum",
  1: "Tarjeta de crédito",
};

function PaymentOrder() {
  const { token } = useAuth();
  const { orderId } = useContext(CheckoutContext);
  const [orderData, setOrderData] = useState(null);
  const equivalentEthereum = getVarLS("equivalentEthereum");
  useEffect(() => {
    if (orderId) updateLocalStorage(orderId, "orderId");

    const fetchOrder = async () => {
      try {
        const data = await orderById(ORDER_BY_ID, orderId, token);
        setOrderData(data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchOrder();
  }, [token, orderId]);

  if (!orderData) return <p>No hay ningun pedido.</p>;

  return (
    <div className={classes.container}>
      <div className={classes.order}>
        <p>Pedido Nº {orderData.id}</p>
        <p>Fecha de facturación: {new Date(orderData.billingDate).toLocaleDateString()}</p>
        <hr className={classes.line} />
      </div>

      <div className={classes.gameList}>
        {orderData.orderGames?.map((game) => (
          <div key={game.gameId} className={classes.gameItem}>
            <Link href={`/juego/${game.gameId}`}>
              <div className={classes.gameListImg}>
                <img
                  src={`${BASE_URL}${game.imageGame.imageUrl}`}
                  alt={game.imageGame.altText}
                  className={classes.listImg}
                />
              </div>
            </Link>
            <div className={classes.gameListData}>
              <p>{game.title}</p>
              <p>{(game.price / 100).toFixed(2)} €</p>
              <p>Cantidad: {game.quantity}</p>
            </div>
          </div>
        ))}
      </div>

      <div className={classes.payment}>
        <hr className={classes.line} />
        <p>Pagado con: {paymentModes[orderData.modeOfPay]}</p>
        <p>Total pagado: {(orderData.totalPrice / 100).toFixed(2).replace(".",",")} €</p>
        {orderData.modeOfPay === 0 && <p>Total Ethereum: {equivalentEthereum} ETH</p>}
      </div>

      <div className={classes.buttons}>
        <Link href="/" className={classes.button}>
          Volver al inicio
        </Link>
        <Link href="/catalogo" className={classes.button}>
          Ver más juegos
        </Link>
      </div>
    </div>
  );
}

export default PaymentOrder;
