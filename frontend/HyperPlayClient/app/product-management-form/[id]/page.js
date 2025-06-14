'use client'

import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import ProductManagementForm from '../../../src/pages/ProductManagementForm'
import { useAuth } from '../../../src/context/AuthContext'

export default function ProductManagementFormPage({ params }) {
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

  return <ProductManagementForm id={params.id} />
} 