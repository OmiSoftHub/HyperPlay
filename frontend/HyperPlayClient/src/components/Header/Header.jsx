'use client'

import { useState, useRef } from "react";
import classes from "./Header.module.css";
import Button from "../Buttons/Button";
import { useRouter } from "next/navigation";
import LoginModal from "../Login/LoginModal";
import RegisterModal from "../Register/RegisterModal";
import CartIcon from "./CartIcon";
import { useAuth } from "../../context/AuthContext";
import { deleteLocalStorage } from "../../utils/keep";

function Header() {
  const [mostrarLogin, setMostrarLogin] = useState(false);
  const [mostrarRegister, setMostrarRegister] = useState(false);
  const [showLogout, setShowLogout] = useState(false);
  const [showAdmin, setShowAdmin] = useState(false);

  const [menuOpen, setMenuOpen] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [drmFilter, setDrmFilter] = useState(-1);
  const [bit, setBit] = useState(0);

  const { token, username, logout, decodedToken } = useAuth();
  const router = useRouter();
  const timerRef = useRef(null);

  // Manejar clic en el ícono de usuario
  const handleUserClick = () => {
    if (!token) {
      setMostrarLogin(true);
    } else {
      router.push("/user");
    }
  };

  // Mostrar el ícono de logout al hacer hover
  const handleMouseEnter = () => {
    if (timerRef.current) {
      clearTimeout(timerRef.current);
      timerRef.current = null;
    }
    setShowLogout(true);
  };

  const handleMouseLeave = () => {
    timerRef.current = setTimeout(() => {
      setShowLogout(false);
    }, 500);
  };

  const handleMouseEnterAdmin = () => {
    if (timerRef.current) {
      clearTimeout(timerRef.current);
      timerRef.current = null;
    }
    setShowAdmin(true);
  };

  const handleMouseLeaveAdmin = () => {
    timerRef.current = setTimeout(() => {
      setShowAdmin(false);
    }, 500);
  };

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  const handleDRMOption = (drmValue) => {
    setDrmFilter((prevFilter) => (prevFilter === drmValue ? -1 : drmValue));
    setMenuOpen(false);
  };

  const toggleBit = () => {
    setBit(previousState => !previousState)
  }

  const handleSearch = () => {
    toggleBit();
    router.push(`/catalogo?SearchText=${encodeURIComponent(searchText)}&DrmFree=${drmFilter}&Bit=${bit}`);
  };

  // Manejar clic en logout
  const handleLogout = () => {
    if (timerRef.current) clearTimeout(timerRef.current);
    logout();
    setShowLogout(false);
    deleteLocalStorage("cart");
    router.push("/");
  };

  return (
    <header className={classes.header}>
      <div className={classes.logo} onClick={() => router.push('/')}>
        <img src="/img/LogoVG.png" alt="VHGames Logo" />
        <h1>VHGames</h1>
      </div>

      <nav className={classes.nav}>
        <a href="/catalogo" className={classes.navLink}>Catálogo</a>
        <a href="/sobre-nosotros" className={classes.navLink}>Sobre Nosotros</a>
      </nav>

      <form className={classes.searchBar} onSubmit={handleSearch}>
        <input
          type="text"
          placeholder="Buscar juegos..."
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
        />
        <img
          src="/icon/search.svg"
          alt="Buscar"
          className={classes.searchIcon}
          onClick={handleSearch}
        />
      </form>

      <div className={classes.userSection}>
        <img
          src="/icon/cart.svg"
          alt="Carrito"
          className={classes.cartIcon}
          onClick={() => router.push('/cart')}
        />

        {token ? (
          <div className={classes.userMenu}>
            <button className={classes.userButton}>
              {decodedToken?.username}
              <img src="/icon/arrow-down.svg" alt="Menu" />
            </button>
            <div className={classes.dropdown}>
              {decodedToken?.role === 'Admin' && (
                <a href="/admin" className={classes.dropdownItem}>
                  Panel Admin
                </a>
              )}
              <a href="/perfil" className={classes.dropdownItem}>
                Mi Perfil
              </a>
              <a href="/pedidos" className={classes.dropdownItem}>
                Mis Pedidos
              </a>
              <button onClick={handleLogout} className={classes.dropdownItem}>
                Cerrar Sesión
              </button>
            </div>
          </div>
        ) : (
          <Button variant="medium" color="morado" onClick={() => router.push('/login')}>
            Iniciar Sesión
          </Button>
        )}
      </div>

      {mostrarLogin && (
        <LoginModal
          onClose={() => setMostrarLogin(false)}
          onRegisterClick={() => setMostrarRegister(true)}
        />
      )}

      {mostrarRegister && (
        <RegisterModal onClose={() => setMostrarRegister(false)} />
      )}
    </header>
  );
}

export default Header;
