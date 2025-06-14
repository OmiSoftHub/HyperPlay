'use client'

import '../src/styles/Global.css'
import '../src/styles/index.css'
import { AuthProvider } from '../src/context/AuthContext'
import { CartProvider } from '../src/context/CartContext'
import { CheckoutProvider } from '../src/context/CheckoutContext'
import { AdminProvider } from '../src/context/AdminContext'
import { ImageProvider } from '../src/context/ImageContext'
import RootLayout from '../src/components/Layout/RootLayout'

export default function Layout({ children }) {
  return (
    <html lang="es">
      <head>
        <title>VHGames - Tu tienda de videojuegos</title>
        <meta name="description" content="Tu tienda de videojuegos favorita" />
      </head>
      <body>
        <AuthProvider>
          <CartProvider>
            <CheckoutProvider>
              <AdminProvider>
                <ImageProvider>
                  <RootLayout>
                    {children}
                  </RootLayout>
                </ImageProvider>
              </AdminProvider>
            </CheckoutProvider>
          </CartProvider>
        </AuthProvider>
      </body>
    </html>
  )
} 