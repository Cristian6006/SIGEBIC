# language: es
  # Author: Cristian

  Característica: Asignar un rol a un usuario
    Como administrador del sistema
    Quiero poder asignar un rol a un usuario
    Para gestionar los permisos del sistema

  Antecedentes:
    Dado el administrador inicie sesion con las credenciales correctas
      | usuario              | contraseña |
      | admin@biblioteca.com | Admin1234  |
    Y se encuentre en la pagina de gestion gestion de Usuarios

  @AsignarRol
  Escenario: Asignar un rol a un usuario
    Dado que existe un usuario sin rol asignado
    Cuando el administrador asigna un rol al usuario
    Entonces deberia ver que el usuario tiene el rol asignado