'use client'

import Checkout from '../../../src/pages/Checkout'

export default function CheckoutPage({ params }) {
  return <Checkout modo={params.modo} />
} 