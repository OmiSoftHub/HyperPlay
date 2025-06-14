'use client'

import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import UsersManagement from '../../src/pages/UsersManagement'
import { useAuth } from '../../src/context/AuthContext'

export default function UsersManagementPage() {
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

  return <UsersManagement />
} 