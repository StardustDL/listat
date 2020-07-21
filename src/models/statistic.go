package models

import (
	"time"
)

type Statistic struct {
	Id               string
	CreationTime     time.Time
	ModificationTime time.Time
	Payload          string
	Uri              string
	Category         string
}

type StatisticQuery struct {
	Id               string
	CreationTime     time.Time
	ModificationTime time.Time
	Payload          string
	Uri              string
	Category         string
	Offset           int
	Limit            int
}
