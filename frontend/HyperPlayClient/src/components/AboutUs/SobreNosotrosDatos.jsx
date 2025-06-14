import classes from "./SobreNosotrosDatos.module.css";

function SobreNosotrosDatos() {
  return (
    <section className={classes.container}>
      <div className={classes.content}>
        <div className={classes.imageContainer}>
          <img src="/img/Foto-oficina.svg" alt="office" />
        </div>
        <div className={classes.textContainer}>
          <div className={classes.text}>
            <img src="/icon/icono-nave.svg" alt="Misión Empresarial" className={classes.iconoShip} />
            <div>
              <h3 className={classes.title}>Misión Empresarial</h3>
              <p>VHGames ofrece una plataforma de e-commerce para que los jugadores adquieran videojuegos digitales, con opciones tanto con licencia como sin DRM. La empresa busca crear una comunidad gamer activa, donde los usuarios disfruten de flexibilidad y control sobre sus compras.</p>
            </div>
          </div>
          <div className={classes.text}>
            <img src="/icon/icono-nave.svg" alt="Visión" className={classes.iconoShip} />
            <div>
              <h3 className={classes.title}>Visión</h3>
              <p>Convertirse en una de las principales plataformas de referencia en la venta de videojuegos digitales en España, reconocida por su libertad del usuario, su innovación en la experiencia de compra y su compromiso con una comunidad gamer activa y apasionada.</p>
            </div>
          </div>
          <div className={classes.text}>
            <img src="/icon/icono-nave.svg" alt="Valores" className={classes.iconoShip} />
            <div>
              <h3 className={classes.title}>Valores</h3>
              <p> La pasión por los videojuegos guía a VHGames en cada decisión. Promovemos la libertad y flexibilidad a través de títulos sin DRM, mientras fomentamos la innovación y personalización en la experiencia del usuario. También educamos sobre el uso ético de los videojuegos y el apoyo a los creadores, y nos comprometemos a integrar a los jugadores en una cultura gamer sólida.</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default SobreNosotrosDatos;
