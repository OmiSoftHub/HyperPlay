'use client'

import classes from './Button.module.css';

/**
 * Button Component
 * 
 * Este componente renderiza un botón personalizado, permitiendo diferentes variantes.
 *
 * @param {Object} props - Propiedades para configurar el componente.
 * @param {React.ReactNode} props.children - Contenido.
 * @param {Function} props.onClick - Función que se ejecuta.
 * @param {string} props.variant - Tamaño (`short`, `large`, `medium`).
 * @param {string} props.color - Color (`azul`, `morado`, `azul-morado`, `morado-azul`).
 * @param {boolean} props.disabled - Indica si el botón está deshabilitado.
 * @param {string} props.type - Tipo de botón (`button`, `submit`, `reset`).
 * 
 * @returns {JSX.Element} Componente `<Button />`.
 * 
 * @example
 * <Button variant="short" color="azul" onClick={function}>Texto</Button>
 */
function Button({ children, variant = 'medium', color = 'morado', onClick, disabled, type = 'button' }) {
  const buttonClasses = [
    classes.button,
    classes[variant],
    classes[color]
  ].join(' ');

  return (
    <button
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
}

export default Button;
