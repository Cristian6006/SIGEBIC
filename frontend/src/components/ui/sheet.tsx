import { cn } from '@/lib/utils';
import { X } from 'lucide-react';
import { forwardRef, type HTMLAttributes } from 'react';

interface SheetProps extends HTMLAttributes<HTMLDivElement> {
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
}

function Sheet({ open, onOpenChange, children, ...props }: SheetProps) {
  if (!open) return null;
  return (
    <div className="fixed inset-0 z-50" {...props}>
      <div
        className="fixed inset-0 bg-black/50 transition-opacity"
        onClick={() => onOpenChange?.(false)}
      />
      {children}
    </div>
  );
}

const SheetContent = forwardRef<
  HTMLDivElement,
  HTMLAttributes<HTMLDivElement> & { side?: 'left' | 'right' }
>(({ className, children, side = 'left', ...props }, ref) => (
  <div
    ref={ref}
    className={cn(
      'fixed z-50 h-full w-72 bg-background shadow-lg transition-transform',
      side === 'left' ? 'left-0' : 'right-0',
      className
    )}
    {...props}
  >
    {children}
  </div>
));
SheetContent.displayName = 'SheetContent';

const SheetHeader = ({ className, ...props }: HTMLAttributes<HTMLDivElement>) => (
  <div className={cn('flex flex-col space-y-2 px-6 py-4', className)} {...props} />
);

const SheetTitle = forwardRef<HTMLHeadingElement, HTMLAttributes<HTMLHeadingElement>>(
  ({ className, ...props }, ref) => (
    <h2 ref={ref} className={cn('text-lg font-semibold', className)} {...props} />
  )
);
SheetTitle.displayName = 'SheetTitle';

const SheetClose = forwardRef<HTMLButtonElement, HTMLAttributes<HTMLButtonElement>>(
  ({ className, ...props }, ref) => (
    <button
      ref={ref}
      className={cn(
        'absolute right-4 top-4 rounded-sm opacity-70 transition-opacity hover:opacity-100',
        className
      )}
      {...props}
    >
      <X className="h-4 w-4" />
      <span className="sr-only">Cerrar</span>
    </button>
  )
);
SheetClose.displayName = 'SheetClose';

export { Sheet, SheetContent, SheetHeader, SheetTitle, SheetClose };