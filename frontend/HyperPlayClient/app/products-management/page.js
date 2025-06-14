'use client'

import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import ProductManagement from '../../src/pages/ProductManagement'
import { useAuth } from '../../src/context/AuthContext'

export default function ProductsManagementPage() {
  const router = useRouter()
  const { isAuthenticated, isAdmin } = useAuth()

  useEffect(() => {
    if (!isAuthenticated || !isAdmin) {
      router.push('/')
    }
  }, [isAuthenticated, isAdmin, router])

  if (!isAuthenticated || !isAdmin) {
    return null
  }

  return <ProductManagement />
} 