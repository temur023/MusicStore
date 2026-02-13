import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  preview: {
    port: process.env.PORT ? parseInt(process.env.PORT) : 8080,
    strictPort: true,
    host: "0.0.0.0",
  },
  server: {
    port: 8080,
    host: "0.0.0.0",
  }
})