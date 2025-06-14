import classes from "./Carousel.module.css";
import CarouselPack from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";
import Link from "next/link";

function Carousel() {
  const responsive = {
    desktop: {
      breakpoint: { max: 3000, min: 1024 },
      items: 1,
      partialVisibilityGutter: 20,
    },
    tablet: {
      breakpoint: { max: 1024, min: 464 },
      items: 1,
      partialVisibilityGutter: 20,
    },
    mobile: {
      breakpoint: { max: 464, min: 0 },
      items: 1,
      partialVisibilityGutter: 20,
    },
  }

  return (
    <div className={classes.carrouselContainer}>
      <CarouselPack
        className={classes.carrousel}
        responsive={responsive}
        infinite={true}
        autoPlay={true}
        autoPlaySpeed={5000}
        showDots={true}
        arrows={false}
      > 
        <Link href="/sobre-nosotros" className={classes.bannerLink}>
          <img className={classes.bannerImg} src="/img/marca2.webp" alt="Marca 2" />
        </Link>
        <Link href="/juego/32" className={classes.bannerLink}>
          <img
            className={classes.bannerImg}
            src="/img/dragon-ball.webp"
            alt="Dragon Ball"
          />
        </Link>
        <Link href="/juego/31" className={classes.bannerLink}>
          <img className={classes.bannerImg} src="/img/gow.webp" alt="GOW" />
        </Link>
        <Link href="/juego/30" className={classes.bannerLink}>
          <img
            className={classes.bannerImg}
            src="/img/minecraft.webp"
            alt="Minecraft"
          />
        </Link>
        <Link href="/juego/1" className={classes.bannerLink}>
          <img
            className={classes.bannerImg}
            src="/img/witcher3.webp"
            alt="Witcher 3"
          />
        </Link>
      </CarouselPack>
    </div>
  );
}

export default Carousel;
