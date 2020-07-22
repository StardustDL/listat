package repositories

import (
	"database/sql"
	"fmt"
	"listat/models"

	// nothing
	_ "github.com/go-sql-driver/mysql"
	"xorm.io/xorm"
	"xorm.io/xorm/names"
)

// StatisticRepository repo
type StatisticRepository struct {
	engine     *xorm.Engine
	dataSource string
	dbName     string
}

// Create repo
func Create(dataSource string, dbName string) *StatisticRepository {
	repo := new(StatisticRepository)
	repo.dbName = dbName
	repo.dataSource = dataSource
	repo.engine = nil
	return repo
}

// Start repo engine
func (repo *StatisticRepository) Start(isDebug bool) error {
	engine, err := xorm.NewEngine("mysql", fmt.Sprintf("%s/%s", repo.dataSource, repo.dbName))
	if err != nil {
		return err
	}
	if isDebug {
		engine.ShowSQL(true)
	}
	engine.SetTableMapper(names.SameMapper{})
	engine.SetColumnMapper(names.SameMapper{})
	engine.Sync2(new(models.Statistic))
	repo.engine = engine
	return err
}

// Stop repo engine
func (repo *StatisticRepository) Stop() error {
	err := repo.engine.Close()
	return err
}

// EnsureExisits db exisis
func (repo *StatisticRepository) EnsureExisits() error {
	db, err := sql.Open("mysql", fmt.Sprintf("%s/", repo.dataSource))
	if err != nil {
		return err
	}
	defer db.Close()
	_, err = db.Exec(fmt.Sprintf("create database if not exists %s", repo.dbName))
	return err
}

// Create new statistic
func (repo *StatisticRepository) Create(obj *models.Statistic) error {
	_, err := repo.engine.Insert(obj)
	return err
}

// Get by id
func (repo *StatisticRepository) Get(id string) (*models.Statistic, error) {
	var result models.Statistic
	has, err := repo.engine.Where("Id = ?", id).Get(&result)
	if has {
		return &result, err
	}
	return nil, err
}

// Update by id
func (repo *StatisticRepository) Update(obj *models.Statistic) error {
	_, err := repo.engine.Where("Id = ?", obj.Id).Update(obj)
	return err
}

// Delete by id
func (repo *StatisticRepository) Delete(id string) (*models.Statistic, error) {
	var obj models.Statistic
	_, err := repo.engine.Where("Id = ?", id).Delete(&obj)
	return &obj, err
}

// Query statistics
func (repo *StatisticRepository) Query(query *models.StatisticQuery) ([]models.Statistic, error) {
	session := repo.engine.NewSession()
	if query.Id != "" {
		session = session.Where("Id = ?", query.Id)
	}
	if query.Uri != "" {
		session = session.Where("Uri = ?", query.Uri)
	}
	if query.Category != "" {
		session = session.Where("Category = ?", query.Category)
	}
	if query.Limit == 0 {
		query.Limit = 10
	}
	var result []models.Statistic
	err := session.Limit(query.Limit, query.Offset).Find(&result)
	return result, err
}

// Count statistics
func (repo *StatisticRepository) Count(query *models.StatisticQuery) (int64, error) {
	session := repo.engine.NewSession()
	if query.Id != "" {
		session = session.Where("Id = ?", query.Id)
	}
	if query.Uri != "" {
		session = session.Where("Uri = ?", query.Uri)
	}
	if query.Category != "" {
		session = session.Where("Category = ?", query.Category)
	}
	if query.Limit == 0 {
		query.Limit = 10
	}
	stat := new(models.Statistic)
	total, err := session.Limit(query.Limit, query.Offset).Count(stat)
	return total, err
}
