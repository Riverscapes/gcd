import React from 'react'
import styles from './ToolsWrapper.module.css'

type ToolCardProps = {
  title: string
  description?: string
  logoUrl?: string
  toolUrl?: string
  imageUrl?: string // Optional image field
  // cardsize?: "sm" | "md" | "lg";
}

interface ToolsWrapperProps {
  cards: ToolCardProps[]
  sectionTitle?: string
  cardsize?: 'sm' | 'md' | 'lg'
}

export const ToolsWrapper: React.FC<ToolsWrapperProps> = ({
  cardsize = 'md', // Default to medium size
  cards = [],
  sectionTitle,
}) => {
  const minWidth = cardsize === 'sm' ? '200px' : cardsize === 'lg' ? '400px' : '300px'

  return (
    <div className={styles.section}>
      {sectionTitle && <h2>{sectionTitle}</h2>}
      <div className={styles.grid} style={{ '--card-min-width': minWidth } as React.CSSProperties}>
        {cards.map((card, index) => (
          <ToolCard
            key={index}
            title={card.title}
            description={card.description}
            logoUrl={card.logoUrl}
            toolUrl={card.toolUrl}
            imageUrl={card.imageUrl}
            // cardsize={cardsize}
          />
        ))}
      </div>
    </div>
  )
}

const ToolCard: React.FC<ToolCardProps> = ({
  title,
  description,
  logoUrl,
  toolUrl,
  imageUrl,
  // cardsize = 'sm',
}) => (
  <a href={toolUrl} className={styles.card}>
    {imageUrl && (
      <div className={styles.cardImageWrapper}>
        <img src={imageUrl} alt={title + ' image'} className={styles.cardImage} />
      </div>
    )}
    <div className={styles.cardHeader}>
      {logoUrl && (
        <div className={styles.logoRowWrapper}>
          <img src={logoUrl} alt={title} className={styles.logoRow} />
        </div>
      )}
      <h3 className={styles.cardTitle}>{title}</h3>
    </div>
    <div className={styles.cardContent}>{description && <p>{description}</p>}</div>
  </a>
)
