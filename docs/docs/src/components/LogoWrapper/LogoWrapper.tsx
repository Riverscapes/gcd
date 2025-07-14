import React from 'react'
import styles from './LogoWrapper.module.css'

export type Logo = {
  imageUrl: string
  name?: string
  url?: string
}

export type LogoWrapperProps = {
  logos: Logo[]
  size?: 'sm' | 'md' | 'lg'
}

const sizeClass = (size: string = 'md') => {
  switch (size) {
    case 'sm':
      return styles.sm
    case 'lg':
      return styles.lg
    case 'md':
    default:
      return styles.md
  }
}

export const LogoWrapper: React.FC<LogoWrapperProps> = ({ logos, size = 'md' }) => {
  return (
    <div className={`${styles.logoWrapper} ${sizeClass(size)}`}>
      {logos.map((logo, idx) => (
        <div className={styles.logoItem} key={idx}>
          {logo.url ? (
            <a href={logo.url} target="_blank" rel="noopener noreferrer">
              <img src={logo.imageUrl} alt={logo.name || 'Logo'} className={styles.logoImg} />
            </a>
          ) : (
            <img src={logo.imageUrl} alt={logo.name || 'Logo'} className={styles.logoImg} />
          )}
          {logo.name && <div className={styles.logoName}>{logo.name}</div>}
        </div>
      ))}
    </div>
  )
}
