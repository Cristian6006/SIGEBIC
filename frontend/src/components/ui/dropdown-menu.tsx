import { cn } from '@/lib/utils';
import {
  useEffect,
  useRef,
  useState,
  type HTMLAttributes,
  type ReactElement,
  type ReactNode,
} from 'react';

interface DropdownMenuProps {
  children: ReactNode;
}

interface DropdownMenuTriggerProps {
  children: ReactElement;
  asChild?: boolean;
}

interface DropdownMenuContentProps extends HTMLAttributes<HTMLDivElement> {
  align?: 'start' | 'end';
}

function DropdownMenu({ children }: DropdownMenuProps) {
  const [open, setOpen] = useState(false);
  return (
    <DropdownMenuContext value={{ open, setOpen }}>
      {children}
    </DropdownMenuContext>
  );
}

interface DropdownMenuContextType {
  open: boolean;
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

const DropdownMenuContext = createSimpleContext<DropdownMenuContextType | null>(null);

const isElement = (val: unknown): val is { props: Record<string, unknown>; type: unknown } =>
  typeof val === 'object' && val !== null && 'props' in val && 'type' in val;

function DropdownMenuTrigger({ children, asChild }: DropdownMenuTriggerProps) {
  const ctx = useDropdownMenuContext();
  const child = children;
  const extra = {
    onClick: (e: React.MouseEvent) => {
      if (isElement(child) && child.props && typeof child.props === 'object' && 'props' in child && child.props) {
        const onClick = (child.props as Record<string, unknown>).onClick;
        if (typeof onClick === 'function') (onClick as (e: React.MouseEvent) => void)(e);
      }
      ctx.setOpen((prev: boolean) => !prev);
    },
  };

  if (asChild && isElement(child)) {
    const childProps = child.props as Record<string, unknown>;
    const mergedOnClick = ((e: React.MouseEvent) => {
      if (typeof childProps.onClick === 'function') (childProps.onClick as (e: React.MouseEvent) => void)(e);
      ctx.setOpen((prev: boolean) => !prev);
    });
    return cloneElementSimple(child, { onClick: mergedOnClick });
  }

  return (
    <span onClick={extra.onClick} className="cursor-pointer">
      {child}
    </span>
  );
}

function DropdownMenuContent({ className, children, align = 'end', ...props }: DropdownMenuContentProps) {
  const ctx = useDropdownMenuContext();
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        ctx.setOpen(false);
      }
    }
    if (ctx.open) {
      document.addEventListener('mousedown', handleClickOutside);
      return () => document.removeEventListener('mousedown', handleClickOutside);
    }
  }, [ctx.open, ctx.setOpen]);

  if (!ctx.open) return null;

  return (
    <div
      ref={ref}
      className={cn(
        'absolute z-50 mt-2 min-w-40 rounded-lg border bg-popover p-1 text-popover-foreground shadow-md',
        align === 'end' ? 'right-0' : 'left-0',
        className
      )}
      {...props}
    >
      {children}
    </div>
  );
}

function DropdownMenuItem({
  className,
  ...props
}: HTMLAttributes<HTMLDivElement>) {
  const ctx = useDropdownMenuContext();
  return (
    <div
      className={cn(
        'relative flex cursor-pointer select-none items-center gap-2 rounded-md px-2 py-1.5 text-sm outline-none transition-colors hover:bg-accent hover:text-accent-foreground',
        className
      )}
      onClick={() => ctx.setOpen(false)}
      {...props}
    />
  );
}

function DropdownMenuSeparator({ className, ...props }: HTMLAttributes<HTMLDivElement>) {
  return <div className={cn('-mx-1 my-1 h-px bg-border', className)} {...props} />;
}

function DropdownMenuLabel({ className, ...props }: HTMLAttributes<HTMLDivElement>) {
  return <div className={cn('px-2 py-1.5 text-sm font-semibold', className)} {...props} />;
}

export {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuLabel,
};

// Internal helpers

import { createContext, useContext } from 'react';

function createSimpleContext<T>(defaultValue: T) {
  const ctx = createContext<T>(defaultValue);
  return ctx;
}

function useDropdownMenuContext(): NonNullable<DropdownMenuContextType> {
  const ctx = useContext(DropdownMenuContext);
  if (!ctx) throw new Error('DropdownMenu components must be used within <DropdownMenu>');
  return ctx;
}

function cloneElementSimple(element: ReactElement, extraProps: Record<string, unknown>) {
  const childProps = element.props as Record<string, unknown> || {};
  const mergedProps = { ...childProps, ...extraProps };
  const Tag = element.type as React.ElementType;
  return <Tag {...mergedProps}>{typeof childProps.children === 'function' ? (childProps.children as () => ReactNode)() : childProps.children}</Tag>;
}