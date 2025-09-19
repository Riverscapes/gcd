import { themes as prismThemes } from 'prism-react-renderer'
import type { Config } from '@docusaurus/types'
import type * as Preset from '@docusaurus/preset-classic'

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'GCD', // Site title displayed in the browser tab
  tagline: 'Geomorphic Change Detection (GCD) Software website', // Short description shown in meta tags
  favicon: 'favicon.ico', // Path to site favicon

  future: {
    v4: true, // Enables compatibility with upcoming Docusaurus v4 features
  },

  url: 'https://gcd.riverscapes.net', // The base URL of your site (no trailing slash)
  baseUrl: '/', // The sub-path where your site is served (used in GitHub Pages)

  onBrokenLinks: 'throw', // Throw an error on broken links
  onBrokenMarkdownLinks: 'warn', // Warn instead of throwing for broken markdown links

  i18n: {
    defaultLocale: 'en', // Default language
    locales: ['en'], // Supported languages
  },

  themes: ['@riverscapes/docusaurus-theme'], // Shared custom theme used across sites

  presets: [
    [
      'classic', // Docusaurus classic preset for docs/blog
      {
        gtag: {
          trackingID: 'G-ZX2LDXQ1CK',
          anonymizeIP: true,
        },
        docs: {
          sidebarPath: './sidebars.ts', // Path to sidebar config
          routeBasePath: '/', // Serve docs at site root
          editUrl: 'https://github.com/Riverscapes/GCD/tree/master/docs', // "Edit this page" link
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: 'img/logo.png', // Social sharing image

    algolia: {
      // The application ID provided by Algolia
      appId: '4TGS8ZPIMY',

      // Public API key: it is safe to commit it
      apiKey: 'd084a7919fe7b5940d7125f14221eaca',

      indexName: 'gcd.riverscapes.net',

      // Optional: see doc section below
      contextualSearch: true,
    },
    navbar: {
      title: 'Geomorphic Change Detection Software', // Navbar title
      logo: {
        alt: 'Riverscapes Studio Logo', // Logo alt text
        src: 'img/logo.png', // Logo image path
      },
      items: [
        // {
        //   type: 'docSidebar',
        //   sidebarId: 'tutorialSidebar', // ID of the sidebar defined in sidebars.ts
        //   position: 'left',
        //   label: 'MENU1', // Label in the navbar
        // },
        {
          href: 'https://github.com/Riverscapes/gcd', // External GitHub link
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      links: [
        {
          // Note that this NEEDS to match what's in the default template or we get another column
          title: 'User Resources',
          items: [
            // {
            //   label: 'Join this User Community',
            //   href: '',
            // },
            {
              label: 'Search the Data Exchange',
              href: 'https://data.riverscapes.net/s?type=Project&projectTypeId=gcd&view=map',
            },
            {
              label: 'Developers & Code Repository',
              href: 'https://github.com/Riverscapes/gcd',
            },
            // {
            //   label: 'Knowledge Base',
            //   href: 'https://riverscapes.freshdesk.com/support/solutions/folders/153000068960',
            // },
          ],
        },
      ],
    },

    prism: {
      theme: prismThemes.github, // Code block theme for light mode
      darkTheme: prismThemes.dracula, // Code block theme for dark mode
    },
  } satisfies Preset.ThemeConfig,
}

export default config
