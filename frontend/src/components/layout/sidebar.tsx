import { NavLink, useLocation } from 'react-router-dom';
import { LayoutDashboard, BookOpen, Users, AlertTriangle, Menu } from 'lucide-react';
import { cn } from '@/lib/utils';
// import { Button } from '@/components/ui/button';
import { Sheet, SheetContent, SheetClose } from '@/components/ui/sheet';

interface SidebarProps {
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
  isMobile?: boolean;
}

const navItems = [
  { to: '/dashboard', label: 'Inicio', icon: LayoutDashboard },
  { to: '/dashboard', label: 'Libros', icon: BookOpen },
  { to: '/dashboard', label: 'Socios', icon: Users },
  { to: '/dashboard', label: 'Alertas', icon: AlertTriangle },
];

export default function Sidebar({ open = false, onOpenChange, isMobile = false }: SidebarProps) {
  const location = useLocation();

  const sidebarContent = (
    <div className="flex h-full flex-col gap-2">
      <div className="flex h-14 items-center border-b px-6">
        <BookOpen className="mr-2 h-6 w-6 text-primary" />
        <span className="text-lg font-bold tracking-tight">SIGEBIC</span>
        {isMobile && (
          <SheetClose className="ml-auto" onClick={() => onOpenChange?.(false)}>
            <Menu className="h-5 w-5" />
          </SheetClose>
        )}
      </div>

      <nav className="flex-1 space-y-1 px-3 py-2">
        {navItems.map((item) => {
          const isActive = location.pathname === item.to;
          const Icon = item.icon;
          return (
            <NavLink
              key={item.label}
              to={item.to}
              onClick={() => isMobile && onOpenChange?.(false)}
              className={cn(
                'flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                isActive
                  ? 'bg-primary/10 text-primary'
                  : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'
              )}
            >
              <Icon className="h-4 w-4" />
              {item.label}
            </NavLink>
          );
        })}
      </nav>

      <div className="border-t px-6 py-3">
        <p className="text-xs text-muted-foreground">v1.0.0</p>
      </div>
    </div>
  );

  if (isMobile) {
    return (
      <Sheet open={open} onOpenChange={onOpenChange}>
        <SheetContent side="left" className="p-0">
          {sidebarContent}
        </SheetContent>
      </Sheet>
    );
  }

  return (
    <aside className="hidden h-screen w-64 shrink-0 border-r bg-card lg:block">
      {sidebarContent}
    </aside>
  );
}