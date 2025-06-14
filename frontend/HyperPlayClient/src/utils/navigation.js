'use client'

import { useRouter, usePathname, useSearchParams } from 'next/navigation'

// Hook to replace useNavigate
export function useNavigate() {
  const router = useRouter()
  return {
    push: (path) => router.push(path),
    replace: (path) => router.replace(path),
    back: () => router.back()
  }
}

// Hook to replace useLocation
export function useLocation() {
  const pathname = usePathname()
  const searchParams = useSearchParams()
  
  return {
    pathname,
    search: searchParams.toString() ? `?${searchParams.toString()}` : '',
    state: null // Next.js doesn't support location state
  }
}

// Note: For useParams, we should use Next.js's built-in params prop in page components
// This is just a fallback for components that need params outside of page components
export function useParams() {
  const pathname = usePathname()
  const params = {}
  
  // Extract dynamic route parameters
  const segments = pathname.split('/')
  segments.forEach((segment, index) => {
    if (segment.startsWith('[') && segment.endsWith(']')) {
      const paramName = segment.slice(1, -1)
      params[paramName] = segments[index + 1]
    }
  })
  
  return params
}

// Helper function to get search params
export function getSearchParams(searchParams) {
  const params = {}
  searchParams.forEach((value, key) => {
    params[key] = value
  })
  return params
} 