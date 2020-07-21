package main

import (
	"fmt"
	"net/http"
	"os"
	"strconv"
	"time"

	"github.com/go-chi/chi"
	"github.com/go-chi/chi/middleware"

	"listat/handlers"
	"listat/repositories"
)

func main() {
	port := 80

	{
		portEnv := os.Getenv("LISTAT_PORT")
		p, err := strconv.Atoi(portEnv)
		if err == nil {
			port = p
		}
	}

	dbSource := os.Getenv("LISTAT_DBORIGIN")
	dbName := os.Getenv("LISTAT_DBNAME")
	debug := os.Getenv("LISTAT_DEBUG")
	repo := repositories.Create(dbSource, dbName)

	err := repo.EnsureExisits()
	if err != nil {
		fmt.Printf("%v\n", err)
		return
	}

	err = repo.Start(debug != "")
	if err != nil {
		fmt.Printf("%v\n", err)
		return
	}

	defer repo.Stop()

	r := chi.NewRouter()

	r.Use(middleware.RequestID)
	r.Use(middleware.RealIP)
	r.Use(middleware.Logger)
	r.Use(middleware.Recoverer)

	r.Use(middleware.Timeout(60 * time.Second))

	handler := new(handlers.StatisticHandler)
	handler.Repo = repo

	r.Route("/", func(r chi.Router) {
		r.Post("/", handler.Create)     // statistic -> id
		r.Post("/query", handler.Query) // statisticQuery -> list[statistic] or null
		r.Route("/{id}", func(r chi.Router) {
			r.Use(handlers.ParamID)
			r.Get("/", handler.Get)       // id -> statistic
			r.Delete("/", handler.Delete) // id -> bool
			r.Put("/", handler.Update)    // id, statistic -> bool
		})
	})

	http.ListenAndServe(fmt.Sprintf(":%d", port), r)
}
