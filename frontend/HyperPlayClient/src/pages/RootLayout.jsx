import Header from "../components/Header/Header";
import ScrollToTop from "../components/ScrollToTop";

function RootLayout({ children }) {
  return (
    <>
      <ScrollToTop />
      <Header />
      <main className="generalContainer">
        {children}
      </main>
    </>
  );
}

export default RootLayout;