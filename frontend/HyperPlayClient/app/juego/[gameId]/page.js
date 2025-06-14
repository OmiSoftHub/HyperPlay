'use client'

import DetailsView from '../../../src/pages/DetailsView'

export default function GameDetailsPage({ params }) {
  return <DetailsView gameId={params.gameId} />
} 