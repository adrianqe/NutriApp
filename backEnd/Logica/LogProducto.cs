using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace backEnd.Logica
{
    public class LogProducto
    {
        public ResActualizarProducto actualizar(ReqActualizarProducto req)
        {
            ResActualizarProducto res = new ResActualizarProducto();

            try
            {

                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto no puede ser nulo");
                }
                else if (req.producto.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else if (string.IsNullOrEmpty(req.producto.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre del producto no puede ser nulo o vacío");
                }
            
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Actualizar_Producto(
                        req.producto.Producto_ID,
                        req.producto.Nombre,
                        req.producto.Categoria,
                        req.producto.Marca,
                        req.producto.Informacion_Nutricional,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);  
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message); 
            }

            return res;
        }

        public ResEliminarProducto eliminar(ReqEliminarProducto req)
        {
            ResEliminarProducto res = new ResEliminarProducto();

            try
            {
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto no puede ser nulo");
                }
                else if (req.producto.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                 
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Eliminar_Producto(
                        req.producto.Producto_ID,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje); 
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  
            }

            return res;
        }

        public ResConsultarProductosEscaneados consultar(ReqConsultarProductosEscaneados req)
        {
            //objeto ResObtenerPublicaciones donde esta la lista
            ResConsultarProductosEscaneados res = new ResConsultarProductosEscaneados();

            //se inicializan las variables de referencia que pide el SP
            bool? exito = false;
            string mensaje = "";

            try
            {
                //conexion al linq
                ConectionDataContext miLinq = new ConectionDataContext();
                //se adaptada al formato del
                //lenguaje de programacion, usando el nombre del procedimiento+Result
                //obtiene la los datos devueltos por la BD
                //en este caso se usa "toList()" para traer una lista
                List<SP_Consultar_Productos_EscaneadosResult> productos = miLinq.SP_Consultar_Productos_Escaneados(req.Usuario_ID, ref exito, ref mensaje).ToList();

                //se pasa por cada una de las entradas de la lista y 
                //se genera el objeto Producto que se pondra en la lista
                //antes mencionada en la linea 81
                foreach (SP_Consultar_Productos_EscaneadosResult unProducto in productos)
                {
                    res.ProductosEscaneados.Add(this.factoriaProducto(unProducto));
                }
                //Si nada fallo se pone exito en true
                res.exito = true;
            }
            catch (Exception ex)
            {
                //Si dios nos abandono se pone el resultado en false
                res.exito = false;
                res.mensaje.Add(ex.Message);
            }

            return res;
        }

        private Producto factoriaProducto(SP_Consultar_Productos_EscaneadosResult productosLinq)
        {
            Producto productoFabricado = new Producto();
            productoFabricado.Nombre = productosLinq.Nombre;
            productoFabricado.Fecha_Escaneo = productosLinq.Fecha_Escaneo;
            return productoFabricado;
        }
    }
}
