# language: es
  # Author: Cristian
Característica: Agregar Usuario
  Como administrador del sistema
  Quiero eliminar usuarios
  Para mantener actualizado el registro del sistema.

  Antecedentes:
  Dado el administrador inicie sesion con las credenciales correctas
    | usuario              | contraseña |
    | admin@biblioteca.com | Admin1234  |
  Y se encuentre en la pagina de gestion gestion de Usuarios

@EliminarUsuario

Escenario: Eliminar un usuario
Dado que existe un usuario
Cuando elimina a el usuario del sistema
Entonces deberia ver que el usuario no exista en la lista