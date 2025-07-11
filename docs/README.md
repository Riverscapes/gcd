# 🧼 Clean Template Site

This project demonstrates the smallest possible Docusaurus site setup. It is designed for simplicity and ease of use, requiring only essential files and folders.

---

## How It Works

- **Write Content:**  
  Add your documentation as Markdown (`.md`) or MDX (`.mdx`) files in the `docs` directory.

- **Add Images:**  
  Place any images you want to use in your docs inside the `img` folder.

- **Custom Components:**  
  You can automatically use React TSX components provided by the [Riverscapes Docusaurus Theme](https://github.com/Riverscapes/riverscapes-docusaurus-theme) directly in your MDX files—no extra setup required.

---

## Why This Structure?

- **No unnecessary files:**  
  Only the core configuration and content folders are included.

- **Easy to maintain:**  
  Just focus on writing docs and adding images.

- **Extendable:**  
  Leverage custom React components for richer documentation.

---

## Getting Started

1. **Install dependencies:**

```bash
yarn install
```

Start the local development server:

```bash
yarn start
```

Build the static site:

```bash
yarn build
```

🔍 View Your Site
Once the dev server is running, visit

http://localhost:3000

This will display your live documentation site with hot reloading enabled.
