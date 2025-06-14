import { NextResponse } from 'next/server'

export function middleware(request) {
  const token = request.cookies.get('token')
  const isAdminRoute = request.nextUrl.pathname.startsWith('/products-management') ||
                      request.nextUrl.pathname.startsWith('/users-management') ||
                      request.nextUrl.pathname.startsWith('/product-management-form')

  if (isAdminRoute && !token) {
    return NextResponse.redirect(new URL('/', request.url))
  }

  return NextResponse.next()
}

export const config = {
  matcher: [
    '/products-management/:path*',
    '/users-management/:path*',
    '/product-management-form/:path*'
  ]
} 