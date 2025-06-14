'use client'

import Header from '../Header/Header'
import Footer from '../Footer/Footer'
import classes from './RootLayout.module.css'

function RootLayout({ children }) {
  return (
    <div className={classes.root}>
      <Header />
      <main className={classes.main}>
        {children}
      </main>
      <Footer />
    </div>
  )
}

export default RootLayout 