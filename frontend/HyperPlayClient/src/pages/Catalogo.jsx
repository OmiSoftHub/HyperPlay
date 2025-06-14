'use client'

import { useState, useEffect } from "react";
import Footer from "../components/Footer/Footer";
import Title from "../components/Titles/Title";
import CatalogBody from "../components/Catalog/CatalogBody";
import { useSearchParams } from "next/navigation";

function Catalogo() {
    const searchParams = useSearchParams();
    const [initialSearchText, setInitialSearchText] = useState("");
    const [initialDrmFree, setInitialDrmFree] = useState(-1);
    const [initialBit, setBit] = useState(0);
    

    useEffect(() => {
        // Extrae los parámetros de la URL cada vez que cambian
        const searchText = searchParams.get("SearchText") || "";
        const drmFree = searchParams.get("DrmFree") !== null ? searchParams.get("DrmFree") : -1;
        const bit = searchParams.get("Bit") !== null ? searchParams.get("Bit") : 0;



        setInitialSearchText(searchText);
        setInitialDrmFree(drmFree);
        setBit(bit);
    }, [searchParams]);
    

    return (
        
        <div>
            <div className="generalContainer">
                <Title text="CATÁLOGO" size="3em" color="#fff" align="center" />
                 <CatalogBody initialSearchText={initialSearchText} initialDrmFree={initialDrmFree} initialBit={initialBit} />
            </div>
            <Footer />
        </div>

    )
}

export default Catalogo;