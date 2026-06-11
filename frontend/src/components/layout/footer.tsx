export default function Footer() {
  return (
    <footer className="border-t bg-background px-6 py-4">
      <div className="flex flex-col items-center justify-between gap-2 text-center sm:flex-row">
        <p className="text-sm text-muted-foreground">
          &copy; {new Date().getFullYear()} Biblioteca Comunitaria &mdash; Gesti&oacute;n Local e Inclusiva
        </p>
        <div className="flex gap-4">
          <a href="#" className="text-xs text-muted-foreground hover:underline">
            Soporte
          </a>
          <a href="#" className="text-xs text-muted-foreground hover:underline">
            T&eacute;rminos
          </a>
        </div>
      </div>
    </footer>
  );
}